// Translated from ChatLine.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 11
    public class ChatLine : com.oddlabs.tt.gui.EditLine
    {
        public java.util.List tab_listeners;
        public bool catch_tab;
        public string[] tab_complete_list;
        // Java: 23
        public void setTabCompleteList(string[] list)
        {
            tab_complete_list = list;
        }
        // Java: 27
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (catch_tab && @event.getCharacter() == '\t')
                {
                    tabComplete(getText());
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 39
        public void tabComplete(java.lang.StringBuilder line)
        {
            int index = getIndex();
            int word_start = line.lastIndexOf(" ", index - 1) + 1;
            if (word_start == (-1))
            {
                word_start = 0;
            }
            int word_end = line.indexOf(" ", index);
            if (word_end == (-1))
            {
                word_end = line.length();
            }
            string partial_word = line.substring(word_start, word_end);
            java.util.List new_words = new java.util.ArrayList();
            int num_hits = 0;
            foreach (string tab_word in tab_complete_list)
            {
                if (tab_word.startsWith(partial_word))
                {
                    num_hits++;
                    new_words.add(tab_word);
                }
            }
            if (num_hits == 1)
            {
                string new_word = ((string)new_words.getFirst());
                line.replace(word_start, word_end, new_word);
                int new_index = word_start + new_word.length();
                setIndex(new_index);
            }
            else if (num_hits > 1)
            {
                string[] tab_words = new string[new_words.size()];
                for (int i = 0; i < new_words.size(); i++)
                {
                    tab_words[i] = ((string)new_words.get(i));
                }
                tabPressedAll(tab_words);
            }
        }
        // Java: 89
        public void tabPressedAll(string[] words)
        {
            tabPressed(words);
            foreach (com.oddlabs.tt.guievent.TabListener listener in java.lang.JIter.of(tab_listeners))
            {
                listener.tabPressed(words);
            }
        }
        // Java: 96
        public void tabPressed(string[] words)
        {
        }
        // Java: 99
        public void addTabListener(com.oddlabs.tt.guievent.TabListener listener)
        {
            tab_listeners.add(listener);
        }
        private void init_ChatLine()
        {
            this.tab_listeners = new java.util.ArrayList();
        }
        public ChatLine(int width, int max_chars, bool catch_tab) : base(width, max_chars)
        {
            init_ChatLine();
            this.catch_tab = catch_tab;
            this.tab_complete_list = new string[0];
        }
    }
}
