'use strict';

class AbstractUnite {
    constructor() {
        this.name = "Abs";

        this.hp = 0;
        this.damage = 0;
        this.max_hp = 0;
        this.cost = 0;
        this.x_range = 0;
        this.y_range = 0;
        this.distance = 0;
        this.speed = 0;
    }
}

class Warrior extends AbstractUnite {
    constructor() {
        super();
        this.name = "Warrior";

        this.hp = 3;
        this.max_hp = 3;
        this.cost = 3;

        this.x_range = 1;
        this.y_range = 1;
        this.damage = 1;
        this.distance = 1;
        this.speed = 1;
    }
}

class StrongWarrior extends AbstractUnite {
    constructor() {
        super();
        this.name = "StrongWarrior";


        this.hp = 5;
        this.max_hp = 5;
        this.cost = 5;

        this.x_range = 1;
        this.y_range = 1;
        this.damage = 2;
        this.distance = 1;
        this.speed = 1;
    }
}


class Healer extends AbstractUnite {
    constructor() {
        super();
        this.name = "Healer";


        this.hp = 1;
        this.max_hp = 1;
        this.cost = 3;

        this.x_range = 3;
        this.y_range = 3;
        this.damage = -1;
        this.distance = 1;
        this.speed = 0;
    }
}

class Producer extends AbstractUnite {
    constructor() {
        super();
        this.name = "Producer";

        this.hp = 1;
        this.max_hp = 1;
        this.cost = 3;

        this.x_range = 0;
        this.y_range = 0;
        this.damage = 0;
        this.distance = 0;
        this.productivity = 1;

    }
}

class Magician extends AbstractUnite {
    constructor() {
        super();
        this.name = "Magician";

        this.hp = 3;
        this.max_hp = 3;
        this.cost = 2;

        this.x_range = 3;
        this.y_range = 1;
        this.damage = 1;
        this.distance = 1;
        this.speed = 1;
    }
}

class Archer extends AbstractUnite {
    constructor() {
        super();
        this.name = "Archer";

        this.hp = 3;
        this.max_hp = 3;
        this.cost = 2;

        this.x_range = 1;
        this.y_range = 1;
        this.damage = 1;
        this.distance = 3;
        this.speed = 1;
    }
}

