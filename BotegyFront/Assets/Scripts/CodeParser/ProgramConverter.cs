using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Model;

namespace CodeParser
{
    public static class ProgramConverter
    {
        public static BlockProgram Convert(string program)
        {
            program = program.Replace("let ", "").Replace("this.field.", "").Replace("'", "");
         
            ICharStream stream = CharStreams.fromString(program);
            ITokenSource lexer = new ParserGrammarLexer(stream);
            ITokenStream tokens = new CommonTokenStream(lexer);
            ParserGrammarParser parser = new ParserGrammarParser(tokens);
            parser.BuildParseTree = true;
            IParseTree tree = parser.program();

            return TreeNodeConverter.Convert(tree);
        }
    }
}