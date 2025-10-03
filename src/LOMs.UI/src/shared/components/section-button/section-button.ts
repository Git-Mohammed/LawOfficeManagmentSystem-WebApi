import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-section-button',
  imports: [],
  templateUrl: './section-button.html',
  styleUrl: './section-button.css'
})
export class SectionButton {
  @Input({required:true}) actionText!:string;
  @Output() action:EventEmitter<void> = new EventEmitter<void>;
}
