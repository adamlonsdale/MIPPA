import { TeamViewModel } from './teamviewmodel';
import { MatchViewModel } from './matchviewmodel';

export class WeekViewModel {
    constructor(public date: string, public time: string, public teamViewModels: Array<TeamViewModel>, public matchViewModels: Array<MatchViewModel>) { }
}