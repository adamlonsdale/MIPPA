import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';

import { Player } from '../../../model/player';

@Component({
  selector: 'cp-player-edit',
  template: require('./player-edit.component.html')
})
export class PlayerEditComponent implements OnInit {
  @Input() player: Player;
  @Output() onCancelPlayer = new EventEmitter<boolean>();
  @Output() onSubmitPlayer = new EventEmitter<Player>();

  constructor() { }

  ngOnInit() {
  }

  onCancel() {
    this.onCancelPlayer.emit(true);
  }

  onSubmit(value: any) {
    this.onSubmitPlayer.emit(
      new Player(0, value.name, value.handicap)
    );
  }

}
