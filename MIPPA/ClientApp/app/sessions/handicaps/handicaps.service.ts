import { Injectable, EventEmitter } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';

import 'rxjs/Rx';

@Injectable()
export class SessionsService {

    databasePath = '/api/handicap';

    constructor(private http: Http) { }

    getHandicapsForSession(sessionId: number) {
        // TODO: Complete this
    }

}
