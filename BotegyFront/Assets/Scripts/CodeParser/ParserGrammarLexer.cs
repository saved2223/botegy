//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.10.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:/Users/sun_t/BotegyApp/Assets/Scripts/CodeParser\ParserGrammar.g4 by ANTLR 4.10.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.10.1")]
[System.CLSCompliant(false)]
public partial class ParserGrammarLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, NUMBER=11, STRING=12, WS=13, ADD=14, SUB=15, MUL=16, DIV=17, 
		MOD=18, AND=19, OR=20, BIT_AND=21, BIT_OR=22, GE=23, LE=24, NEQUALS=25, 
		EQUALS=26, GT=27, LT=28;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE"
	};

	public static readonly string[] ruleNames = {
		"T__0", "T__1", "T__2", "T__3", "T__4", "T__5", "T__6", "T__7", "T__8", 
		"T__9", "WORD", "NUMBER", "STRING", "WS", "ADD", "SUB", "MUL", "DIV", 
		"MOD", "AND", "OR", "BIT_AND", "BIT_OR", "GE", "LE", "NEQUALS", "EQUALS", 
		"GT", "LT"
	};


	public ParserGrammarLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public ParserGrammarLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'('", "','", "')'", "'='", "'{'", "'}'", "'if'", "'else'", "'while'", 
		"';'", null, null, null, "'+'", "'-'", "'*'", "'/'", "'%'", "'&&'", "'||'", 
		"'&'", "'|'", "'>='", "'<='", "'!='", "'=='", "'>'", "'<'"
	};
	private static readonly string[] _SymbolicNames = {
		null, null, null, null, null, null, null, null, null, null, null, "NUMBER", 
		"STRING", "WS", "ADD", "SUB", "MUL", "DIV", "MOD", "AND", "OR", "BIT_AND", 
		"BIT_OR", "GE", "LE", "NEQUALS", "EQUALS", "GT", "LT"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "ParserGrammar.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static ParserGrammarLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,28,157,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,
		6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,13,2,14,
		7,14,2,15,7,15,2,16,7,16,2,17,7,17,2,18,7,18,2,19,7,19,2,20,7,20,2,21,
		7,21,2,22,7,22,2,23,7,23,2,24,7,24,2,25,7,25,2,26,7,26,2,27,7,27,2,28,
		7,28,1,0,1,0,1,1,1,1,1,2,1,2,1,3,1,3,1,4,1,4,1,5,1,5,1,6,1,6,1,6,1,7,1,
		7,1,7,1,7,1,7,1,8,1,8,1,8,1,8,1,8,1,8,1,9,1,9,1,10,1,10,1,11,4,11,91,8,
		11,11,11,12,11,92,1,11,1,11,4,11,97,8,11,11,11,12,11,98,3,11,101,8,11,
		1,12,1,12,3,12,105,8,12,1,12,1,12,1,12,5,12,110,8,12,10,12,12,12,113,9,
		12,1,13,4,13,116,8,13,11,13,12,13,117,1,13,1,13,1,14,1,14,1,15,1,15,1,
		16,1,16,1,17,1,17,1,18,1,18,1,19,1,19,1,19,1,20,1,20,1,20,1,21,1,21,1,
		22,1,22,1,23,1,23,1,23,1,24,1,24,1,24,1,25,1,25,1,25,1,26,1,26,1,26,1,
		27,1,27,1,28,1,28,0,0,29,1,1,3,2,5,3,7,4,9,5,11,6,13,7,15,8,17,9,19,10,
		21,0,23,11,25,12,27,13,29,14,31,15,33,16,35,17,37,18,39,19,41,20,43,21,
		45,22,47,23,49,24,51,25,53,26,55,27,57,28,1,0,2,2,0,65,90,97,122,3,0,9,
		10,13,13,32,32,163,0,1,1,0,0,0,0,3,1,0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,
		1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,15,1,0,0,0,0,17,1,0,0,0,0,19,1,0,0,
		0,0,23,1,0,0,0,0,25,1,0,0,0,0,27,1,0,0,0,0,29,1,0,0,0,0,31,1,0,0,0,0,33,
		1,0,0,0,0,35,1,0,0,0,0,37,1,0,0,0,0,39,1,0,0,0,0,41,1,0,0,0,0,43,1,0,0,
		0,0,45,1,0,0,0,0,47,1,0,0,0,0,49,1,0,0,0,0,51,1,0,0,0,0,53,1,0,0,0,0,55,
		1,0,0,0,0,57,1,0,0,0,1,59,1,0,0,0,3,61,1,0,0,0,5,63,1,0,0,0,7,65,1,0,0,
		0,9,67,1,0,0,0,11,69,1,0,0,0,13,71,1,0,0,0,15,74,1,0,0,0,17,79,1,0,0,0,
		19,85,1,0,0,0,21,87,1,0,0,0,23,90,1,0,0,0,25,104,1,0,0,0,27,115,1,0,0,
		0,29,121,1,0,0,0,31,123,1,0,0,0,33,125,1,0,0,0,35,127,1,0,0,0,37,129,1,
		0,0,0,39,131,1,0,0,0,41,134,1,0,0,0,43,137,1,0,0,0,45,139,1,0,0,0,47,141,
		1,0,0,0,49,144,1,0,0,0,51,147,1,0,0,0,53,150,1,0,0,0,55,153,1,0,0,0,57,
		155,1,0,0,0,59,60,5,40,0,0,60,2,1,0,0,0,61,62,5,44,0,0,62,4,1,0,0,0,63,
		64,5,41,0,0,64,6,1,0,0,0,65,66,5,61,0,0,66,8,1,0,0,0,67,68,5,123,0,0,68,
		10,1,0,0,0,69,70,5,125,0,0,70,12,1,0,0,0,71,72,5,105,0,0,72,73,5,102,0,
		0,73,14,1,0,0,0,74,75,5,101,0,0,75,76,5,108,0,0,76,77,5,115,0,0,77,78,
		5,101,0,0,78,16,1,0,0,0,79,80,5,119,0,0,80,81,5,104,0,0,81,82,5,105,0,
		0,82,83,5,108,0,0,83,84,5,101,0,0,84,18,1,0,0,0,85,86,5,59,0,0,86,20,1,
		0,0,0,87,88,7,0,0,0,88,22,1,0,0,0,89,91,2,48,57,0,90,89,1,0,0,0,91,92,
		1,0,0,0,92,90,1,0,0,0,92,93,1,0,0,0,93,100,1,0,0,0,94,96,5,46,0,0,95,97,
		2,48,57,0,96,95,1,0,0,0,97,98,1,0,0,0,98,96,1,0,0,0,98,99,1,0,0,0,99,101,
		1,0,0,0,100,94,1,0,0,0,100,101,1,0,0,0,101,24,1,0,0,0,102,105,3,21,10,
		0,103,105,5,95,0,0,104,102,1,0,0,0,104,103,1,0,0,0,105,111,1,0,0,0,106,
		110,3,21,10,0,107,110,5,95,0,0,108,110,3,23,11,0,109,106,1,0,0,0,109,107,
		1,0,0,0,109,108,1,0,0,0,110,113,1,0,0,0,111,109,1,0,0,0,111,112,1,0,0,
		0,112,26,1,0,0,0,113,111,1,0,0,0,114,116,7,1,0,0,115,114,1,0,0,0,116,117,
		1,0,0,0,117,115,1,0,0,0,117,118,1,0,0,0,118,119,1,0,0,0,119,120,6,13,0,
		0,120,28,1,0,0,0,121,122,5,43,0,0,122,30,1,0,0,0,123,124,5,45,0,0,124,
		32,1,0,0,0,125,126,5,42,0,0,126,34,1,0,0,0,127,128,5,47,0,0,128,36,1,0,
		0,0,129,130,5,37,0,0,130,38,1,0,0,0,131,132,5,38,0,0,132,133,5,38,0,0,
		133,40,1,0,0,0,134,135,5,124,0,0,135,136,5,124,0,0,136,42,1,0,0,0,137,
		138,5,38,0,0,138,44,1,0,0,0,139,140,5,124,0,0,140,46,1,0,0,0,141,142,5,
		62,0,0,142,143,5,61,0,0,143,48,1,0,0,0,144,145,5,60,0,0,145,146,5,61,0,
		0,146,50,1,0,0,0,147,148,5,33,0,0,148,149,5,61,0,0,149,52,1,0,0,0,150,
		151,5,61,0,0,151,152,5,61,0,0,152,54,1,0,0,0,153,154,5,62,0,0,154,56,1,
		0,0,0,155,156,5,60,0,0,156,58,1,0,0,0,8,0,92,98,100,104,109,111,117,1,
		6,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}