import {
  AfterViewInit,
  Directive,
  ElementRef,
  Input,
  OnInit,
  Renderer2,
} from '@angular/core';

@Directive({
  selector: '[appEmployeeStatusStyle]',
})
export class EmployeeStatusStyleDirective implements OnInit, AfterViewInit {
  @Input('appEmployeeStatusStyle') isActive!: boolean;

  constructor(private el: ElementRef, private renderer: Renderer2) {}
  ngAfterViewInit(): void {
    this.renderer.setStyle(this.el.nativeElement, 'textAlign', 'center');
    this.renderer.setStyle(this.el.nativeElement, 'borderRadius', '50px');
    this.renderer.setStyle(this.el.nativeElement, 'padding', '0.1rem 2rem');
    this.renderer.setStyle(this.el.nativeElement, 'fontWeight', '500');
    this.renderer.setStyle(
      this.el.nativeElement,
      'boxShadow',
      'rgba(0, 0, 0, 0.2) 0px 2px 6px'
    );
  }

  ngOnInit(): void {
    if (this.isActive) {
      this.el.nativeElement.style.backgroundColor = '#d4edda'; // light green
      this.el.nativeElement.style.color = '#155724'; // dark green
    } else {
      this.el.nativeElement.style.backgroundColor = '#f8d7da'; // light red
      this.el.nativeElement.style.color = '#721c24'; // dark red
    }
  }
}
