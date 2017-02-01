using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;





namespace WebRole1
{
    public class Trie
    {
        private static TrieNode root;
        private static int hybridCapacity = 2;

        /// constructor
        public Trie()
        {
            root = new TrieNode();
        }


        public String insert(String word)
        {
            if (word == null || word.Length == 0)
            {
                return "can't do empty";
            }
            return rearrange(root, word);
        }

        public String rearrange(TrieNode current, String tempWord)
        {
            current.HybridList.Add(tempWord);
            //if passed in word's length is 1, then mark node as end of word
            if (tempWord.Length == 0)
            {
                current.SetEndOfWord(true);
                return "done";
            }

            if (current.HybridList.Count <= hybridCapacity)
            {
                return "done";
            }
            else
            {
                foreach (String item in current.HybridList)
                {
                    //get first letter in item
                    char ch = item[0];
                    TrieNode node;

                    //check that first letter is in the dictionary key
                    if (current.Dict.ContainsKey(ch))
                    {
                        //if it does, pass that word into that node
                        node = current.Dict[ch];
                    }
                    else
                    {
                        //if it doesn't, create a new node with that letter and add the rest of the letters into the hybrid
                        node = new TrieNode();
                        current.Dict.Add(ch, node);
                    }
                    if (item.Length > 1)
                    {
                        rearrange(node, item.Substring(1, item.Length - 1));
                    }
                }
                current.HybridList.Clear();
                return "done";
            }
        }

        public List<String> search(String input)
        {
            List<String> list = new List<String>();
            if (input == null || input.Length == 0)
            {
                return list;
            }
            TrieNode current = searchPrefix(root, input, 0);

            /// if passed in prefix doesn't exist, then return empty list
            if (current == null)
            {
                return list;
            }
            else
            {
                return searchSuggestions(current, input, list);
            }
        }

        private TrieNode searchPrefix(TrieNode current, String prefix, int index)
        {
            if (index == prefix.Length)
            {
                return current;
            }
            char ch = prefix[index];
            /// if passed in prefix doesn't exist
            if (!current.Dict.ContainsKey(ch))
            {
                return null;
            }
            else
            {
                TrieNode node = current.Dict[ch];
                return searchPrefix(node, prefix, index + 1);
            }
        }

        private List<String> searchSuggestions(TrieNode current, String tempWord, List<String> list)
        {
            if (list.Count >= 10)
            {
                return list;
            }
            else
            {
                TrieNode node = current;
                if (current.EndOfWord)
                {
                    list.Add(tempWord);
                }

                foreach (String word in current.HybridList)
                {
                    list.Add(tempWord + word);
                    if (list.Count >= 10)
                    {
                        return list;
                    }
                }

                //if
                foreach (KeyValuePair<char, TrieNode> item in node.Dict)
                {
                    char ch = item.Key;
                    TrieNode nextNode = item.Value;
                    list = searchSuggestions(nextNode, tempWord + ch, list);
                }
                return list;
            }
        }

    }
}