import { Injectable, EventEmitter } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';

import 'rxjs/Rx';

@Injectable()
export class HandicapsService {

    databasePath = '/api/handicap';

    constructor(private http: Http) { }

    getHandicapsForSession(sessionId: number) {
        return this.http.get(this.databasePath + '/' + sessionId)
        .map(res => res.json())
    }

    updateHandicapForPlayer(sessionId: number, playerId: number, handicap: number) {
        const body = JSON.stringify({ playerId: playerId, handicap: handicap });
        const headers = new Headers(
            {
                'Content-Type': 'application/json'
            }
        );

        return this.http.put(this.databasePath + '/' + playerId + '/' + sessionId, body, { headers: headers });
    }

}
