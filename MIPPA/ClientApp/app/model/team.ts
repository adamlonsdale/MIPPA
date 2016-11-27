import { Player } from './player';

export class Team {
    constructor(public teamId: number, public name: string, public sessionId: number, public players: Player[], public bye: boolean) {}
}
