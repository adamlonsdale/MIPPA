import { Player } from '../../model/player';

export class PlayerMatchViewModel {
    playerMatchId: number;
    homePlayer: Player;
    homePlayerScore: number;
    awayPlayer: Player;
    awayPlayerScore: number;
    homeBreaking: boolean;
    saved: boolean;
    validationError: boolean;
}