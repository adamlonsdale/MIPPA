import { Player } from './player';

export class Team {
    constructor(public name: string, public players: Player[], public bye: boolean) {}
}
