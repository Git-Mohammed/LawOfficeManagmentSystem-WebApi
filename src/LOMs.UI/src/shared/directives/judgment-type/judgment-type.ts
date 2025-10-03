import { AfterViewInit, Directive, ElementRef, Renderer2 } from '@angular/core';
import { enJudgmentType } from '../../enums/enums';

@Directive({
  selector: '[appJudgmentStyle]',
})
export class JudgmentStyleDirective implements AfterViewInit {
  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit(): void {
    this.applyJudgmentStyle();
  }

  private applyJudgmentStyle() {
    const degree = this.el.nativeElement.innerText.trim();

    this.renderer.setStyle(this.el.nativeElement, 'textAlign', 'center');
    this.renderer.setStyle(this.el.nativeElement, 'borderRadius', '50px');
    this.renderer.setStyle(this.el.nativeElement, 'padding', '0.1rem 2rem');
    this.renderer.setStyle(this.el.nativeElement, 'fontWeight', '500');
    this.renderer.setStyle(this.el.nativeElement, 'boxShadow', 'rgba(0, 0, 0, 0.2) 0px 2px 6px');

    switch (degree as enJudgmentType) {
      case enJudgmentType.FTDegree:
        this.setHSL(210, 70, 90, 210, 70, 30); // light blue
        break;
      case enJudgmentType.SEDegree:
        this.setHSL(160, 65, 88, 160, 65, 28); // mint green
        break;
      case enJudgmentType.THDegree:
        this.setHSL(45, 90, 85, 45, 90, 30); // amber
        break;
      case enJudgmentType.FRDegree:
        this.setHSL(0, 70, 90, 0, 70, 30); // soft red
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
