grammar ParserGrammar;
fragment WORD: 'a'..'z' | 'A'..'Z' ;
NUMBER: ('0'..'9')+ ('.' ('0'..'9')+)? ;
STRING: ( WORD | '_' ) ( WORD | '_' | NUMBER )*;

WS: [ \r\n\t]+ -> skip;

ADD: '+';
SUB: '-';
MUL: '*';
DIV: '/';
MOD: '%';
AND: '&&';
OR: '||';
BIT_AND: '&';
BIT_OR: '|';
GE: '>=';
LE: '<=';
NEQUALS: '!=';
EQUALS: '==';
GT: '>';
LT: '<';

ident: STRING;

call: ident '(' (expr ( ','  expr)* )? ')';

group: 
    NUMBER #Value
    | '(' expr ')' #Braces  
    | call #Function
    | ident #Identifier;

mult: group
    | mult ( MUL | DIV | MOD ) group;

add: mult
    | add ( ADD | SUB ) mult;

compare1: add
    | add ( GT | LT | GE | LE ) add;

compare2: compare1
    | compare1 ( EQUALS | NEQUALS ) compare1;

logical_and: compare2
    | logical_and AND compare2;

logical_or: logical_and
    | logical_or OR logical_and;

expr: logical_or;

assign : ident '=' expr;

simple_stmt: assign
    | call;

composite: '{' stmt_list '}';

if: 'if' '(' expr ')' stmt ('else' stmt)?;

while: 'while' '(' expr ')' stmt;

stmt: simple_stmt ';'
    | if
    | while
    | composite;
    
stmt_list: (stmt';'*)*;

program: stmt_list;
