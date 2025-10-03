import {
  AfterViewInit,
  Directive,
  ElementRef,
  HostListener,
  OnInit,
  Renderer2,
} from '@angular/core';

@Directive({
  selector: '[appCaseStatus]',
  standalone: true,
})
export class CaseStatus implements AfterViewInit {
  constructor(private el: ElementRef, private renderer: Renderer2) {}
  ngAfterViewInit(): void {
    this.highlight();
  }

  private highlight() {
    this.renderer.setStyle(this.el.nativeElement, 'textAlign', 'center');
    this.renderer.setStyle(this.el.nativeElement, 'borderRadius', '50px');
    this.renderer.setStyle(this.el.nativeElement, 'padding', '0.1rem 2rem');
    this.renderer.setStyle(this.el.nativeElement, 'color', 'white');
    // this.renderer.setStyle(this.el.nativeElement, 'boxShadow', 'rgba(0, 0, 0, 0.35) 0px 5px 15px');
    this.customColor();
  }

  private customColor() {
    switch (this.el.nativeElement.innerText) {
      case 'قيد نظر': // حالة انتظار أو مراجعة
        this.renderer.setStyle(
          this.el.nativeElement,
          'backgroundColor',
          '#fdf3d5'
        ); // أصفر فاتح
        this.renderer.setStyle(this.el.nativeElement, 'color', '#b38b00'); // أصفر غامق
        break;

      case 'قيد منتهي': // منجز
        this.renderer.setStyle(
          this.el.nativeElement,
          'backgroundColor',
          '#d6f5d6'
        ); // أخضر فاتح
        this.renderer.setStyle(this.el.nativeElement, 'color', '#1e7d1e'); // أخضر غامق
        break;

      case 'قيد تنفيذ': // شغال الآن
        this.renderer.setStyle(
          this.el.nativeElement,
          'backgroundColor',
          '#cfe2ff'
        ); // أزرق فاتح
        this.renderer.setStyle(this.el.nativeElement, 'color', '#084298'); // أزرق غامق
        break;

      case 'ملغي': // غير صالح
        this.renderer.setStyle(
          this.el.nativeElement,
          'backgroundColor',
          '#f8d7da'
        ); // أحمر وردي
        this.renderer.setStyle(this.el.nativeElement, 'color', '#842029'); // أحمر غامق
        break;

      case 'مسودة': // غير مكتمل
        this.renderer.setStyle(
          this.el.nativeElement,
          'backgroundColor',
          '#e2e3e5'
        ); // رمادي فاتح
        this.renderer.setStyle(this.el.nativeElement, 'color', '#41464b'); // رمادي غامق
        break;

      default:
        break;
    }
  }
}
