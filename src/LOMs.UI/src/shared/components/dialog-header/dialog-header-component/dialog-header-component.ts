import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-dialog-header-component',
  imports: [],
  templateUrl: './dialog-header-component.html',
  styleUrl: './dialog-header-component.css'
})
export class DialogHeaderComponent {
  @Input({required:true}) title: string = '';
  @Input() icon?: string;
  @Output() onClose: EventEmitter<void>;
  constructor() {
    this.onClose = new EventEmitter<void>;    
  }
}