import { Injectable, EventEmitter } from '@angular/core';
import { Http, Headers, Response, URLSearchParams } from '@angular/http';

import { Observable }     from 'rxjs/Observable';

import 'rxjs/Rx';

@Injectable()
export class TestingService {

    databasePath = '/api/??';

    constructor(private http: Http) { }

}
