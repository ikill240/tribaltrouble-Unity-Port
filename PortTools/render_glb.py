# Minimal software renderer for converted TT .glb files: skins a frame, textures it,
# applies TT's team-colour rule  rgb = mix(base, teamColor*light, decal.rgb)  and draws it.
import sys, json, struct, numpy as np
from PIL import Image, ImageDraw
def load(glb):
    d=open(glb,'rb').read(); jl=struct.unpack('<I',d[12:16])[0]; js=json.loads(d[20:20+jl]); b=d[20+jl+8:]
    def acc(i):
        a=js['accessors'][i]; bv=js['bufferViews'][a['bufferView']]
        dt={5126:np.float32,5123:np.uint16,5125:np.uint32}[a['componentType']]
        n={'SCALAR':1,'VEC2':2,'VEC3':3,'VEC4':4,'MAT4':16}[a['type']]
        return np.frombuffer(b,dt,a['count']*n,bv['byteOffset']).reshape(a['count'],n)
    return js,acc
def q2m(q):
    x,y,z,w=q; return np.array([[1-2*(y*y+z*z),2*(x*y-z*w),2*(x*z+y*w)],[2*(x*y+z*w),1-2*(x*x+z*z),2*(y*z-x*w)],[2*(x*z-y*w),2*(y*z+x*w),1-2*(x*x+y*y)]])
def sample(js,acc,anim,t):
    nodes=js['nodes']; local={i:(np.array(n.get('translation',[0,0,0])),np.array(n.get('rotation',[0,0,0,1])),np.array(n.get('scale',[1,1,1]))) for i,n in enumerate(nodes)}
    if anim is not None:
        an=[a for a in js['animations'] if a['name']==anim][0]
        for ch in an['channels']:
            s=an['samplers'][ch['sampler']]; ti=acc(s['input'])[:,0]; out=acc(s['output']).astype(np.float64)
            k=np.searchsorted(ti,t,side='right')-1; k=min(max(k,0),len(ti)-1); k2=min(k+1,len(ti)-1)
            f=0 if k2==k else (t-ti[k])/(ti[k2]-ti[k]); v=out[k]*(1-f)+out[k2]*f
            if ch['target']['path']=='rotation': v/=np.linalg.norm(v)
            tr,q,sc=local[ch['target']['node']]
            local[ch['target']['node']]={'translation':(v,q,sc),'rotation':(tr,v,sc),'scale':(tr,q,v)}[ch['target']['path']]
    parent={c:i for i,n in enumerate(nodes) for c in n.get('children',[])}
    world={}
    def W(n):
        if n not in world:
            t,q,s=local[n]; m=np.eye(4); m[:3,:3]=q2m(q)*s; m[:3,3]=t
            world[n]=W(parent[n])@m if n in parent else m
        return world[n]
    return W
def render(glb, tex, decal, anim, t, team=(0.1,0.35,1.0), size=360, lod=0, yaw=30):
    js,acc=load(glb); W=sample(js,acc,anim,t)
    prim=js['meshes'][lod]['primitives'][0]['attributes']; P=acc(prim['POSITION']).astype(np.float64); N=acc(prim['NORMAL']).astype(np.float64)
    UV=acc(prim['TEXCOORD_0']); I=acc(js['meshes'][lod]['primitives'][0]['indices'])[:,0].reshape(-1,3)
    if 'skins' in js:
        sk=js['skins'][0]; ibm=acc(sk['inverseBindMatrices']).reshape(-1,4,4).transpose(0,2,1)
        M=np.stack([W(j)@ibm[i] for i,j in enumerate(sk['joints'])]); J=acc(prim['JOINTS_0']); Wt=acc(prim['WEIGHTS_0'])
        Ph=np.c_[P,np.ones(len(P))]; P2=np.zeros_like(P); N2=np.zeros_like(N)
        for k in range(4):
            P2+=Wt[:,k,None]*np.einsum('nij,nj->ni',M[J[:,k]],Ph)[:,:3]; N2+=Wt[:,k,None]*np.einsum('nij,nj->ni',M[J[:,k]][:,:3,:3],N)
        P,N=P2,N2/np.linalg.norm(N2,axis=1,keepdims=True).clip(1e-9)
    a=np.radians(yaw); R=np.array([[np.cos(a),0,np.sin(a)],[0,1,0],[-np.sin(a),0,np.cos(a)]]); P=P@R.T; N=N@R.T
    TA=np.asarray(Image.open(tex).convert('RGBA'),dtype=np.float64)/255; T=TA[...,:3]; D=np.asarray(Image.open(decal).convert('RGB').resize((T.shape[1],T.shape[0])),dtype=np.float64)/255 if decal else None
    lo,hi=P.min(0),P.max(0); sc=size*0.85/max(hi[0]-lo[0],hi[1]-lo[1]); cx=(lo[0]+hi[0])/2; cy=(lo[1]+hi[1])/2
    sx=(P[:,0]-cx)*sc+size/2; sy=size/2-(P[:,1]-cy)*sc; z=P[:,2]
    img=np.full((size,size,3),0.93); zb=np.full((size,size),-1e9); L=np.array([0.4,0.7,0.6]); L/=np.linalg.norm(L)
    for tri in I:
        xs,ys=sx[tri],sy[tri]; x0,x1=int(max(xs.min(),0)),int(min(xs.max()+1,size-1)); y0,y1=int(max(ys.min(),0)),int(min(ys.max()+1,size-1))
        if x1<x0 or y1<y0: continue
        gx,gy=np.meshgrid(np.arange(x0,x1+1)+.5,np.arange(y0,y1+1)+.5)
        d=(ys[1]-ys[2])*(xs[0]-xs[2])+(xs[2]-xs[1])*(ys[0]-ys[2])
        if abs(d)<1e-12: continue
        w0=((ys[1]-ys[2])*(gx-xs[2])+(xs[2]-xs[1])*(gy-ys[2]))/d; w1=((ys[2]-ys[0])*(gx-xs[2])+(xs[0]-xs[2])*(gy-ys[2]))/d; w2=1-w0-w1
        m=(w0>=0)&(w1>=0)&(w2>=0)
        if not m.any(): continue
        zz=w0*z[tri[0]]+w1*z[tri[1]]+w2*z[tri[2]]; yy,xx=gy[m].astype(int),gx[m].astype(int); zz=zz[m]
        ok=zz>zb[yy,xx]; yy,xx,zz=yy[ok],xx[ok],zz[ok]
        if not len(yy): continue
        ww=np.stack([w0[m][ok],w1[m][ok],w2[m][ok]],1); uv=ww@UV[tri].astype(np.float64); n=ww@N[tri]
        tx=(uv[:,0]%1*(T.shape[1]-1)).astype(int); ty=(uv[:,1]%1*(T.shape[0]-1)).astype(int)   # glTF: v=0 is the top row
        keep=TA[ty,tx,3]>=0.5; yy,xx,zz,ww,n,tx,ty=yy[keep],xx[keep],zz[keep],ww[keep],n[keep],tx[keep],ty[keep]
        if not len(yy): continue
        light=(0.45+0.55*np.clip((n/np.linalg.norm(n,axis=1,keepdims=True).clip(1e-9))@L,0,1))[:,None]
        c=T[ty,tx]*light
        if D is not None: c=c*(1-D[ty,tx])+np.array(team)*light*D[ty,tx]
        zb[yy,xx]=zz; img[yy,xx]=c
    return Image.fromarray((img.clip(0,1)*255).astype(np.uint8))
if __name__=='__main__':
    glb,tex,decal,out=sys.argv[1:5]; shots=[(a.split('@')[0],float(a.split('@')[1])) for a in sys.argv[5:]]
    ims=[render(glb,tex,decal if decal!='-' else None,a,t) for a,t in shots]
    sheet=Image.new('RGB',(360*len(ims),390),'white'); dr=ImageDraw.Draw(sheet)
    for i,(im,(a,t)) in enumerate(zip(ims,shots)): sheet.paste(im,(360*i,30)); dr.text((360*i+10,8),f'{a} t={t}',fill='black')
    sheet.save(out)
