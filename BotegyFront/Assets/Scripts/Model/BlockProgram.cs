using System.Collections.Generic;

namespace Model
{
    public class BlockProgram
    {
        private List<ICode> blocks;

        public BlockProgram(List<ICode> blocks)
        {
            this.blocks = blocks;
        }

        public BlockProgram()
        {
            blocks = new List<ICode>();
        }

        public BlockProgram(string program)
        {
            blocks = new List<ICode>();
        }

        public string GetString()
        {
            Dictionary<string, VariableBlock> variables = new Dictionary<string, VariableBlock>();

            string str = "";

            foreach (var c in blocks)
            {
                str += c.GetString(variables);
                if (c.GetType() == typeof(FunctionBlock))
                    str += ";";

                str += "\n";
            }

            return str;
        }

        public void AddBlock(ICode codeBlock)
        {
            blocks.Add(codeBlock);
        }

        public ICode GetByIndex(int index)
        {
            return blocks[index];
        }

        public void DeleteByIndex(int index)
        {
            blocks.RemoveAt(index);
        }

        public List<ICode> GetBlocks()
        {
            return blocks;
        }

        public void SetBlocks(List<ICode> blocks)
        {
            this.blocks = blocks;
        }
    }
}