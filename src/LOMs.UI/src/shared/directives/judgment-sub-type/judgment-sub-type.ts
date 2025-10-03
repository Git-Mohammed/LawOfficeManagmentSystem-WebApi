import { AfterViewInit, Directive, ElementRef, Renderer2 } from '@angular/core';
import { enJudgmentSubType } from '../../enums/enums';

@Directive({
  selector: '[appJudgmentSubTypeStyle]',
})
export class JudgmentSubTypeStyleDirective implements AfterViewInit {
  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit(): void {
    this.applySubTypeStyle();
  }

  private applySubTypeStyle() {
    const subType = this.el.nativeElement.innerText.trim();

    this.renderer.setStyle(this.el.nativeElement, 'textAlign', 'center');
    this.renderer.setStyle(this.el.nativeElement, 'borderRadius', '50px');
    this.renderer.setStyle(this.el.nativeElement, 'padding', '0.1rem 2rem');
    this.renderer.setStyle(this.el.nativeElement, 'fontWeight', '500');
    this.renderer.setStyle(this.el.nativeElement, 'boxShadow', 'rgba(0, 0, 0, 0.2) 0px 2px 6px');

    switch (subType as enJudgmentSubType) {
      case enJudgmentSubType.normal:
        this.setHSL(0, 0, 95, 0, 0, 30); // رمادي فاتح
        break;
      case enJudgmentSubType.special:
        this.setHSL(45, 100, 85, 30, 100, 25); // ذهبي/برتقالي
        break;
      default:
        this.setHSL(0, 0, 90, 0, 0, 30); // fallback رمادي
    }
  }

  private setHSL(hBg: number, sBg: number, lBg: number, hText: number, sText: number, lText: number) {
    const bg = `hsl(${hBg}, ${sBg}%, ${lBg}%)`;
    const color = `hsl(${hText}, ${sText}%, ${lText}%)`;
    this.renderer.setStyle(this.el.nativeElement, 'backgroundColor', bg);
    this.renderer.setStyle(this.el.nativeElement, 'color', color);
  }
}
