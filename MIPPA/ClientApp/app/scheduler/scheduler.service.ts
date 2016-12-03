import { Injectable } from '@angular/core'
import { Http, Response, Headers, URLSearchParams } from '@angular/http';
import { Observable }     from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/share';
import 'rxjs/add/operator/timeout';
import 'rxjs/add/operator/retryWhen';

import { WeekViewModel } from '../model/weekviewmodel';

@Injectable()
export class SchedulerService {
    private headers: Headers;

    constructor(public http: Http) {
        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');
    }

    GetWeekViewModel(sessionId: number, scheduleIndex: number): Observable<WeekViewModel> {
        return this.http.get('/api/scheduler/' + sessionId + '/' + scheduleIndex)
            .map(res => res.json());
    }
}