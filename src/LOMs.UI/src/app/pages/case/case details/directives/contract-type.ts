import { AfterViewInit, Directive, ElementRef, Renderer2 } from '@angular/core';
import { enContractType } from '../../../../../shared/enums/contract-types';

@Directive({
  selector: '[appContractType]',
})
export class ContractType implements AfterViewInit {
  constructor(private el: ElementRef, private renderer: Renderer2) {}
  ngAfterViewInit(): void {
    this.highlight();
  }

  private highlight() {
    this.renderer.setStyle(this.el.nativeElement, 'textAlign', 'center');
    this.renderer.setStyle(this.el.nativeElement, 'borderRadius', '50px');
    this.renderer.setStyle(this.el.nativeElement, 'padding', '0.1rem 2rem');
    this.renderer.setStyle(this.el.nativeElement, 'color', 'white');
    this.renderer.setStyle(
      this.el.nativeElement,
      'boxShadow',
      'rgba(0, 0, 0, 0.35) 0px 5px 15px'
    );
    this.textColor();
  }

  private textColor() {
    switch (this.el.nativeElement.innerText as enContractType) {
      case enContractType.FixedTerm: // عقد محدد المدة
        this.renderer.setStyle(
          this.el.nativeElement,
          'backgroundColor',
          '#e2e3e5'
        ); // رمادي فاتح
        this.renderer.setStyle(this.el.nativeElement, 'color', '#41464b'); // رمادي غامق
        break;

      case enContractType.OpenEnded: // عقد غير محدد المدة
        this.renderer.setStyle(
          this.el.nativeElement,
          'backgroundColor',
          '#fff3cd'
        ); // أصفر فاتح
        this.renderer.setStyle(this.el.nativeElement, 'color', '#856404'); // أصفر غامق
        break;

      default: // عقد تجريبي
        this.renderer.setStyle(
          this.el.nativeElement,
          'backgroundColor',
          '#cfe2ff'
        ); // أزرق فاتح
        this.renderer.setStyle(this.el.nativeElement, 'color', '#084298'); // أز
    }
  }
}
