using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;





namespace WebRole1
{
    public class Trie
    {
        private static TrieNode root;
        private static int hybridCapacity = 20;

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
                    //if it's the end of the word
                    if (item.Length == 1)
                    {
                        node.SetEndOfWord(true);
                    }

                    if (item.Length >= 2)
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
            List<String> result = new List<String>(); //change list name to result
            TrieNode current = root;
            String tempWord = "";
            char ch = '_';
            if (input == null || input.Length == 0)
            {
                return result;
            }
            for (int i = 0; i < input.Length; i++)
            {
                foreach (String word in current.HybridList)
                //for (int j = 0; j < current.HybridList.Count; j++)
                {
                    //String word = current.HybridList[i];
                    String subtractedWord = input;
                    if (tempWord != "")
                    {
                        subtractedWord = input.Replace(tempWord, "");
                    }
                    if (word.StartsWith(subtractedWord))
                    {
                        result.Add(tempWord + word);
                        if (result.Count == 10)
                        {
                            return result;
                        }
                    }
                }
                ch = input[i];
                if (!current.Dict.ContainsKey(ch))
                {
                    return result;
                }
                else
                {
                    tempWord += ch;
                    current = current.Dict[ch];
                }
            }
            return searchSuggestions(current, tempWord, result, input);
            //return list;
        }

        private List<String> searchSuggestions(TrieNode current, String tempWord, List<String> result, String input) //change order
        {
            if (result.Count == 10)
            {
                return result;
            }
            else
            {
                TrieNode node = current;
                if (current.EndOfWord)
                {
                    if (result.Count < 10)
                    {
                        result.Add(tempWord);
                    }
                }

                foreach (KeyValuePair<char, TrieNode> item in node.Dict)
                // for (int i = 0; i < node.Dict.Count; i++)
                {
                    // String item = node.Dict[i];
                    char ch = item.Key;
                    TrieNode nextNode = item.Value;
                    result = searchSuggestions(nextNode, tempWord + ch, result, input);
                    if (result.Count == 10)
                    {
                        return result;
                    }
                }

                //for (int i = 0; i < current.HybridList.Count; i++)
                foreach (String word in current.HybridList)
                {
                    //String word = current.HybridList[i];
                    result.Add(tempWord + word);
                    if (result.Count == 10)
                    {
                        return result;
                    }
                }
                return result;
            }
        }
    }
}
