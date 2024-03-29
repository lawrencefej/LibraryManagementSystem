import { Directive, Input, OnDestroy, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { Subject } from 'rxjs';
import { take } from 'rxjs/operators';
import { LoginUserDto } from 'src/dto/models';
import { AuthService } from '../_services/authentication.service';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @Input() appHasRole: string[] = [];
  isVisible = false;
  user!: LoginUserDto;

  constructor(
    authService: AuthService,
    private readonly templateRef: TemplateRef<any>,
    private readonly viewContainerRef: ViewContainerRef
  ) {
    authService.loggedInUser$.pipe(take(1)).subscribe(user => (this.user = user));
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    // clear view if no roles
    if (!this.user?.role || this.user == null) {
      this.viewContainerRef.clear();
      return;
    }

    if (this.appHasRole.includes(this.user.role)) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainerRef.clear();
    }
  }
}
