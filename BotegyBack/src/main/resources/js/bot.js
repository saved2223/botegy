class Bot {
    constructor() {
        this.energy = 10;
        this.hp = 30;
    }
}

function create_bot(func) {
    let bot = new Bot();

    bot['behaviour'] = new Function(func);

    return bot;
}
