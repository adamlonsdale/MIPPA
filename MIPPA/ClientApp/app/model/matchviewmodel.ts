import { TeamViewModel } from './teamviewmodel';

export class MatchViewModel {
    constructor(public homeTeamViewModel: TeamViewModel, public awayTeamViewModel: TeamViewModel, public location: string, public table: string) { }
}