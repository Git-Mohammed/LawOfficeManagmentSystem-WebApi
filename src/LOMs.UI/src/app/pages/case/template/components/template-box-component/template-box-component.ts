import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IFile, ITemplateBox } from '../../../../../../core/models/requests';

@Component({
  selector: 'app-template-box-component',
  imports: [],
  templateUrl: './template-box-component.html',
  styleUrl: './template-box-component.css'
})
export class TemplateBoxComponent {
  @Input({required:true}) header:string = '';
  @Output() download:EventEmitter<void> = new EventEmitter<void>;

}
