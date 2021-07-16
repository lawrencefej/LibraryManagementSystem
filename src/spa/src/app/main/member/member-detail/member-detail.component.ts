import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';

import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/_services/basket.service';
import { Checkout } from 'src/app/_models/checkout';
import { CheckoutService } from 'src/app/_services/checkout.service';
import { FeeService } from 'src/app/_services/fee.service';
import { MatTableDataSource } from '@angular/material/table';
import { MemberComponent } from '../member/member.component';
import { NotificationService } from 'src/app/_services/notification.service';
import { Photo } from 'src/app/_models/photo';
import { PhotoService } from 'src/app/_services/photo.service';
import { User } from 'src/app/_models/user';
import { CheckoutForListDto } from 'src/dto/models/checkout-for-list-dto';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('fileInput') myInputVariable: ElementRef;
  displayedColumns = ['title', 'until', 'status', 'action'];
  member: User;
  checkouts: CheckoutForListDto[];
  dataSource = new MatTableDataSource<CheckoutForListDto>();
  public basketItems$: Observable<CheckoutForListDto[]> = of([]);
  public basketItems: Checkout[] = [];

  constructor(
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private notify: NotificationService,
    private photoService: PhotoService,
    private feeService: FeeService,
    private basketService: BasketService,
    private checkoutService: CheckoutService
  ) {
    // this.basketItems$ = this.basketService.getItemsInBasket();
    // this.basketItems$.subscribe((_) => (this.basketItems = _));
  }

  ngOnInit() {
    this.route.data.subscribe(res => {
      this.member = res.member;
    });
    this.getCheckoutsForMember();
  }

  canDeactivate() {
    if (this.basketItems.length > 0) {
      this.notify.warn('The Member Basket has to be empty before you can leave this page');
      return false;
    } else {
      return true;
    }
  }

  getCheckoutsForMember() {
    this.checkoutService.getCheckoutsForCard(this.member.id).subscribe(
      checkouts => {
        this.checkouts = checkouts;
        this.dataSource.data = checkouts;
      },
      error => {
        this.notify.error(error);
      }
    );
  }

  public updateMember(element: any) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '640px';
    dialogConfig.data = element;
    this.dialog.open(MemberComponent, dialogConfig);
  }

  updatePhoto(event) {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      const fd = new FormData();
      fd.append('userId', this.member.id.toString());
      fd.append('file', file);
      this.photoService.changeMemberPhoto(fd).subscribe(
        (res: Photo) => {
          this.member.photoUrl = res.url;
          this.notify.success('Photo changed successfully');
        },
        error => {
          this.notify.error(error);
        }
      );
    }
    this.myInputVariable.nativeElement.value = '';
  }

  returnAsset(checkout: Checkout) {
    this.notify
      .confirm('Are you sure you want to return ' + checkout.title)
      .afterClosed()
      .subscribe(res => {
        if (res) {
          this.checkoutService.returnAsset(checkout.id).subscribe(
            () => {
              this.notify.success(checkout.title + 'was returned successfully');
              this.getCheckoutsForMember();
            },
            error => {
              this.notify.error(error);
            }
          );
        }
      });
  }

  payFees(member: User) {
    this.notify
      .confirm('Are you sure you want to pay $' + member.fees)
      .afterClosed()
      .subscribe(res => {
        if (res) {
          this.feeService.payFees(member.libraryCardNumber).subscribe(
            () => {
              this.notify.success('Payment was successful');
              this.member.fees = 0;
            },
            error => {
              this.notify.error(error);
            }
          );
        }
      });
  }
}
