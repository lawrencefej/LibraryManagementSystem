import { Directive, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { environment } from 'src/environments/environment';

@Directive({
  selector: '[appIsDev]'
})
export class IsDevDirective implements OnInit {
  constructor(private readonly viewContainerRef: ViewContainerRef, private readonly templateRef: TemplateRef<any>) {}

  ngOnInit(): void {
    if (environment.production) {
      this.viewContainerRef.clear();
    } else {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    }
  }
}
