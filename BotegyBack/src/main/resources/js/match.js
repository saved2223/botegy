function get_match_results(first_func, second_func) {
    let field = new Field();

    let bot1 = create_bot(first_func);
    let bot2 = create_bot(second_func);

    bot1.field = field;
    bot2.field = field;

    field.player1 = bot1;
    field.player2 = bot2;

    let id = field.play();

    return id;
}