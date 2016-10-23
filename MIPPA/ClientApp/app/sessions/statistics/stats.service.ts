import { Injectable, EventEmitter } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';

import { Session } from '../../model/session';

import { Observable } from 'rxjs/Rx';

@Injectable()
export class StatsService {

    databasePath = '/api/session';

    constructor(private http: Http) { }

    GetStatistics(sessionId: number, format: number): Observable<any> {
        return this.http.get('api/statistics/' + sessionId + '/' + format).map(res => res.json());
    }

    getSession(sessionId: number): Observable<Session> {
        return this.http.get('api/statistics/' + sessionId).map(res => res.json());
    }

}
