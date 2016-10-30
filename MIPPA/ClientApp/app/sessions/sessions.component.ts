import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Subscription } from 'rxjs/Rx';

import { SessionsService } from './sessions.service';

@Component({
  selector: 'cp-sessions',
  template: require('./sessions.component.html')
})
export class SessionsComponent implements OnInit, OnDestroy {
    private subscription: Subscription;
    managerId: number;
  
  constructor(private sessionsService: SessionsService, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {

      this.subscription =
          this.activatedRoute.params.subscribe(
              (params: any) => {
                  this.managerId = +params['managerId']

                  this.sessionsService.fetchSessions(this.managerId);
              }
          );
  }

  ngOnDestroy() {
      this.subscription.unsubscribe();
  }
}
