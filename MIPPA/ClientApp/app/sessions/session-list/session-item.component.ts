import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Session } from '../../model/session';
import { Team } from '../../model/team';

import { ScheduleViewModel } from '../../viewmodel/schedule/schedule-view-model';

@Component({
  selector: 'cp-session-item',
  template: require('./session-item.component.html')
})
export class SessionItemComponent implements OnInit {
  @Input() session: Session;
  @Input() sessionId: number;

  editingSession: boolean;

  constructor(private router: Router, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
  }

  onEdit(session: Session) {
    this.editingSession = true;
  }

  onManageSession() {
    this.router.navigate([this.sessionId, 'manage', 'teams'], { relativeTo: this.activatedRoute });
  }

  getFormat() {
    if (this.session.format == 0) {
      return "8-Ball";
    }
    else if (this.session.format == 1) {
      return "9-Ball";
    }
    else if (this.session.format == 2) {
      return "8-Ball and 9-Ball";
    }
  }

  getMatchup() {
    if (this.session.matchupType == "0") {
      return "3 on 3";
    }
    else if (this.session.matchupType == "1") {
      return "4 on 4";
    }
    else if (this.session.matchupType == "2") {
      return "5 on 4";
    }
    else if (this.session.matchupType == "3") {
      return "5 on 5";
    }
  }

  onSessionSaved(data: any) {
    this.editingSession = false;
  }

  onSessionCancel() {
    this.editingSession = false;
  }

}
