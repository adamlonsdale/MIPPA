import { Team } from './team';
import { ScheduleViewModel } from '../viewmodel/schedule/schedule-view-model';

export class Session {
    constructor(
        public sessionId: number,
        public managerId: number,
        public name: string, 
        public format: number, 
        public matchupType: string, 
        public teams: Team[], 
        public schedules: any[]) {}
}
