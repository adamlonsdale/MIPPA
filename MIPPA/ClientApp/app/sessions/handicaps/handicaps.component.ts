import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Subscription } from 'rxjs/Rx';

@Component({
    selector: 'cp-handicaps',
    template: require('./handicaps.component.html')
})
export class HandicapsComponent implements OnInit {
    private subscription: Subscription;
    sessionId: number;

    constructor(
        private router: Router,
        private activatedRoute: ActivatedRoute) {
    }

    ngOnInit() {
            this.subscription =
                this.activatedRoute.parent.params.subscribe(
                    (params: any) => {
                        this.sessionId = +params['sessionId']
                        // Do something
                    }
                );
    }

}
