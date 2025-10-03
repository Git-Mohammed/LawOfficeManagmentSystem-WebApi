import { AfterViewInit, Directive, ElementRef, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appRoleStyle]',
})
export class RoleStyleDirective implements AfterViewInit {
  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit(): void {
    this.applyRoleStyle();
  }
  
  private applyRoleStyle() {
    const role = this.el.nativeElement.innerText.trim();
    
    this.renderer.setStyle(this.el.nativeElement, 'textAlign', 'center');
    this.renderer.setStyle(this.el.nativeElement, 'borderRadius', '50px');
    this.renderer.setStyle(this.el.nativeElement, 'padding', '0.1rem 2rem');
    this.renderer.setStyle(this.el.nativeElement, 'fontWeight', '500');
    this.renderer.setStyle(this.el.nativeElement, 'boxShadow', 'rgba(0, 0, 0, 0.2) 0px 2px 6px');

    switch (role) {
      case 'مدير عام':
        this.setHSL(220, 80, 90, 220, 80, 30); // blue
        break;
      case 'مدير تنفيذي':
        this.setHSL(170, 70, 85, 170, 70, 25); // teal
        break;
      case 'مستشار':
        this.setHSL(130, 60, 85, 130, 60, 25); // green
        break;
      case 'محامي':
        this.setHSL(280, 60, 90, 280, 60, 30); // purple
        break;
      default:
        this.setHSL(0, 0, 90, 0, 0, 30); // neutral gray
    }
  }

  private setHSL(hBg: number, sBg: number, lBg: number, hText: number, sText: number, lText: number) {
    const bg = `hsl(${hBg}, ${sBg}%, ${lBg}%)`;
    const color = `hsl(${hText}, ${sText}%, ${lText}%)`;
    this.renderer.setStyle(this.el.nativeElement, 'backgroundColor', bg);
    this.renderer.setStyle(this.el.nativeElement, 'color', color);
  }
}
