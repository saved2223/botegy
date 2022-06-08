class Field {
    constructor() {
        this.field1 = null;
        this.field2 = null;
        this.damage = null;

        this.fortress_damage = 0;

        this.player1 = null;
        this.player2 = null;

        this.turn = 0;

        this.step = 0;
    }

    play() {
        this.text = "Начало матча.\n";

        this.field1 = new Array(8);
        this.field2 = new Array(8);

        for (let i = 0; i < this.field1.length; i++) {
            this.field1[i] = new Array(4);
            this.field2[i] = new Array(4);

            for (let j = 0; j < 4; j++) {
                this.field1[i][j] = null;
                this.field2[i][j] = null;

            }
        }

        while (true) {
            if (this.step >= 40) {
                this.write_result(2);
                return 2;
            }

            this.step += 1;

            if (this.turn) this.player2.behaviour(); else this.player1.behaviour();

            this.attack();

            let id = this.check_field();

            if (id != -1) {
                this.write_result(id);
                return id;
            }

            this.move_forward();
            this.turn = !this.turn;
        }
    }

    attack() {
        this.calc_field_damage();
        this.apply_damage();
    }

    check_field() {
        let r1 = this.check_player_field(this.field1);
        let r2 = this.check_player_field(this.field2);

        if (this.turn) {
            if (this.player1.hp <= 0) {
                return 1;
            }
        } else {
            if (this.player2.hp <= 0) {
                return 0;
            }
        }

        if (r1 && r2) {
            return 2;
        }
        return -1;
    }

    move_forward() {
        let curr_field = this.turn ? this.field2 : this.field1;
        let opponent_field = this.turn ? this.field1 : this.field2;
        let sign = this.turn ? -1 : 1;

        const visited = new Array(8);

        for (let i = 0; i < visited.length; i++) {
            visited[i] = new Array(4);

            for (let j = 0; j < 4; j++) {
                visited[i][j] = false;
            }
        }

        for (let row = 0; row < 8; row++) {
            for (let column = 0; column < 4; column++) {
                if (!visited[row][column]) {
                    visited[row][column] = true;
                    if (curr_field[row][column] != null) {
                        let r = row + 1 * sign * curr_field[row][column].speed;
                        if (0 <= r && r <= 7) {

                            if (curr_field[r][column] == null && opponent_field[r][column] == null) {
                                curr_field[r][column] = curr_field[row][column];
                                curr_field[row][column] = null;
                                visited[r][column] = true;
                            }
                        }
                    }

                }
            }
        }
    }

    calc_field_damage() {
        const curr_field = this.turn ? this.field2 : this.field1;
        const curr_player = this.turn ? this.player2 : this.player1;

        this.damage = new Array(8);

        for (let i = 0; i < this.damage.length; i++) {
            this.damage[i] = new Array(4);

            for (let j = 0; j < 4; j++) {
                this.damage[i][j] = 0;
            }
        }

        this.fortress_damage = 0;

        for (let row = 0; row < 8; row++) {
            for (let column = 0; column < 4; column++) {

                if (curr_field[row][column] != null) {

                    if (curr_field[row][column].productivity != undefined) {
                        curr_player.energy += curr_field[row][column].productivity;
                    }

                    this.calc_damage(curr_field[row][column], column, row);

                }
            }
        }
    }

    apply_damage() {
        let opponent_field = this.turn ? this.field1 : this.field2;
        let current_field = this.turn ? this.field2 : this.field1;

        for (let i = 0; i < 8; i++) {
            for (let j = 0; j < 4; j++) {
                if (this.damage[i][j] < 0) {
                    if (current_field[i][j] != null) {
                        if (current_field[i][j].hp - this.damage[i][j] <= current_field[i][j].max_hp) {
                            current_field[i][j].hp -= this.damage[i][j];

                            this.write_heal(i, j, current_field[i][j]);
                        }
                    }

                } else {
                    if (opponent_field[i][j] != null) {
                        opponent_field[i][j].hp -= this.damage[i][j];
                        if (this.damage[i][j] > 0)
                            this.write_attack(i, j, opponent_field[i][j]);
                        if (opponent_field[i][j].hp <= 0) {
                            this.write_unite_death(i, j, opponent_field[i][j]);
                            opponent_field[i][j] = null;
                        }
                    }

                }
            }
        }
        if (this.turn) this.player1.hp -= this.fortress_damage; else this.player2.hp -= this.fortress_damage;
        if (this.fortress_damage > 0)
            this.write_fortress_attack();
    }


    check_player_field(curr_field) {
        for (let i = 0; i < 8; i++) {
            for (let j = 0; j < 4; j++) {
                if (curr_field[i][j] != null) {
                    return false;
                }
            }

        }
        return true;

    }

    calc_damage(unite, x, y) {
        let x_coord = 0;
        let y_coord = 0;
        for (let i = 0; i < unite.x_range; i++) {
            for (let j = 0; j < unite.y_range; j++) {
                x_coord = x + (i - Math.floor(unite.x_range / 2));

                if (this.turn) y_coord = y - unite.distance - (j - Math.floor(unite.y_range / 2)); else y_coord = y + unite.distance + (j - Math.floor(unite.y_range / 2));


                if (x_coord > 3 || x_coord < 0) continue;

                if (this.turn) {
                    if (y_coord < 0) {
                        if (unite.damage > 0) {
                            this.fortress_damage += unite.damage;
                        }
                        continue;
                    }

                    if (y_coord > 7) continue;
                } else {
                    if (y_coord > 7) {
                        if (unite.damage > 0) this.fortress_damage += unite.damage;
                        continue;
                    }

                    if (y_coord < 0) continue;
                }
                this.damage[y_coord][x_coord] += unite.damage;
            }
        }
    }


    write_attack(i, j, unite) {
        this.text += "Атака игроком " + (this.turn + 1)
            + ": юнит " + unite.name
            + ", клетка (" + i + ", " + j + "), " +
            "ед. урона " + this.damage[i][j] + "\n";

    }

    write_heal(i, j, unite) {
        this.text += "Лечение игроком " + (this.turn + 1)
            + ": юнит " + unite.name
            + ", клетка (" + i + ", " + j + "), " +
            "ед. здоровья " + (this.damage[i][j] * -1) + "\n";
    }

    write_fortress_attack() {
        this.text += "Атака крепости игроком " + (this.turn + 1)
            + ": ед. урона " + this.fortress_damage + "\n";
    }

    write_unite_death(i, j, unite) {
        this.text += "Выход из строя юнита игрока " + (this.turn + 1)
            + ": юнит " + unite.name
            + ", клетка (" + i + ", " + j + ")\n";
    }

    write_result(result){
        this.text += "Матч завершился. ";

        if (result == 2) {
            this.text += "Ничья.";
        }
        else {
            this.text += (result == 0) ? "Победа игрока 1" : "Победа игрока 2";
        }
    }

    put(unite, x, y) {
        if (0 <= x <= 3 && 0 <= y <= 3) {
            if (!this.turn) {
                if (this.field1[y][x] == null) {
                    let new_unite = eval("new " + unite + "()");

                    if (this.player1.energy >= new_unite.cost) {
                        this.field1[y][x] = new_unite;
                        this.player1.energy -= new_unite.cost;
                    }
                }
            } else {
                if (this.field2[Math.abs(y - 7)][Math.abs(x - 3)] == null) {
                    let new_unite = eval("new " + unite + "()");

                    if (this.player2.energy >= new_unite.cost) {
                        this.field2[Math.abs(y - 7)][Math.abs(x - 3)] = new_unite;
                        this.player2.energy -= new_unite.cost;
                    }
                }
            }
        }
    }

    get_self_hp() {
        if (this.turn) return this.player2.hp; else return this.player1.hp;
    }

    get_opponent_hp() {
        if (this.turn) return this.player1.hp; else return this.player2.hp;
    }

    get_unite(x, y) {
        let return_unite = null;
        if (0 <= x <= 3 && 0 <= y <= 7) {
            if (this.field1[y][x]) return_unite = this.field1[y][x];

            if (this.field2[y][x]) return_unite = this.field2[y][x];
        }
        return return_unite;
    }

    get_unite_hp(x, y) {
        let u = this.get_unite(x, y);

        if (u != null) return u.hp;
        return -1;
    }

    is_empty(x, y) {
        return this.field1[y][x] == null && this.field2[y][x] == null;
    }

    get_step_number() {
        return this.step;
    }
}
