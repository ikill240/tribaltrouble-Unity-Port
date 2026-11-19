# Compare glTF skinning (from node TRS keyframes) with TT's Java formula v' = sum w (F I^-1) v.
import sys, json, struct, os, numpy as np, xml.etree.ElementTree as ET
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import tt_geometry_to_gltf as T
glb, geom, sprite_key = sys.argv[1], sys.argv[2], sys.argv[3]
d=open(glb,'rb').read(); jl=struct.unpack('<I',d[12:16])[0]; js=json.loads(d[20:20+jl]); b=d[20+jl+8:]
def acc(i):
    a=js['accessors'][i]; bv=js['bufferViews'][a['bufferView']]
    dt={5126:np.float32,5123:np.uint16,5125:np.uint32}[a['componentType']]
    n={'SCALAR':1,'VEC2':2,'VEC3':3,'VEC4':4,'MAT4':16}[a['type']]
    return np.frombuffer(b,dt,a['count']*n,bv['byteOffset']).reshape(a['count'],n)
def q2m(q):
    x,y,z,w=q; return np.array([[1-2*(y*y+z*z),2*(x*y-z*w),2*(x*z+y*w)],[2*(x*y+z*w),1-2*(x*x+z*z),2*(y*z-x*w)],[2*(x*z-y*w),2*(y*z+x*w),1-2*(x*x+y*y)]])
nodes=js['nodes']; skin=js['skins'][0]; joints=skin['joints']; ibm=acc(skin['inverseBindMatrices']).reshape(-1,4,4).transpose(0,2,1)
parent={c:i for i,n in enumerate(nodes) for c in n.get('children',[])}
prim=js['meshes'][0]['primitives'][0]['attributes']
P=acc(prim['POSITION']).astype(np.float64); J=acc(prim['JOINTS_0']); W=acc(prim['WEIGHTS_0']).astype(np.float64)
root=ET.parse(os.path.join(geom,'geometry.xml')).getroot()
g,s=sprite_key.split('/'); sp=[x for x in root.find(f"group[@name='{g}']").findall('sprite') if x.get('name')==s][0]
skel=T.Skeleton(os.path.join(geom,sp.find('skeleton').text.strip()))
worst=0
for ai,an in enumerate(js['animations']):
    ael=sp.findall('animation')[ai]; fr={int(f.get('index')):T.parse_frame(f) for f in ET.parse(os.path.join(geom,ael.text.strip())).getroot().findall('frame')}
    for fi in [0, len(fr)//2, len(fr)-1]:
        local={}
        for n in joints:
            nd=nodes[n]; t=np.array(nd['translation']); q=np.array(nd['rotation']); sc=np.array(nd['scale'])
            local[n]=(t,q,sc)
        for ch in an['channels']:
            smp=an['samplers'][ch['sampler']]; out=acc(smp['output']); n=ch['target']['node']; t,q,sc=local[n]
            v=out[fi].astype(np.float64)
            if ch['target']['path']=='translation': t=v
            elif ch['target']['path']=='rotation': q=v
            else: sc=v
            local[n]=(t,q,sc)
        world={}
        def W_(n):
            if n in world: return world[n]
            t,q,sc=local[n]; m=np.eye(4); m[:3,:3]=q2m(q)*sc; m[:3,3]=t
            world[n]= (W_(parent[n]) @ m) if parent.get(n) in local else m
            return world[n]
        skinm=np.stack([W_(n)@ibm[i] for i,n in enumerate(joints)])
        Ph=np.c_[P,np.ones(len(P))]
        gl=sum(W[:,k,None]*np.einsum('nij,nj->ni',skinm[J[:,k]],Ph)[:,:3] for k in range(4))
        # TT reference in TT space, then converted
        tt=np.zeros_like(P)
        Ptt=np.stack([P[:,0],-P[:,2],P[:,1]],1)
        for k in range(4):
            mats=np.stack([fr[fi][skel.order[j]] @ T.safe_inv(skel.init_pose[skel.order[j]]) for j in J[:,k]])
            tt+=W[:,k,None]*np.einsum('nij,nj->ni',mats,np.c_[Ptt,np.ones(len(P))])[:,:3]
        ref=T.conv_vec(tt)
        err=np.abs(gl-ref).max(); worst=max(worst,err)
    print(f"{an['name']:14s} max vertex error {err:.2e} m")
print('WORST', worst)
