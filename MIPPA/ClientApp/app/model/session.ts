import { Team } from './team';
import { ScheduleViewModel } from '../viewmodel/schedule/schedule-view-model';

export class Session {
    constructor(
        public name: string, 
        public format: string, 
        public matchupType: string, 
        public teams: Team[], 
        public schedules: any[]) {}
}
