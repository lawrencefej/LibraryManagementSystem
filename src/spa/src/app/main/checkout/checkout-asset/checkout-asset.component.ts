import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, of, Subject } from 'rxjs';

import { AssetService } from 'src/app/_services/asset.service';
import { BasketService } from 'src/app/_services/basket.service';
import { Checkout } from 'src/app/_models/checkout';
import { LibraryAsset } from 'src/app/_models/libraryAsset';
import { MatTableDataSource } from '@angular/material/table';
import { User } from 'src/app/_models/user';
import { LibraryCardForDetailedDto } from 'src/dto/models';
import { BasketViewModel } from '../../basket/models/basket-view-model';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'lms-checkout-asset',
  templateUrl: './checkout-asset.component.html',
  styleUrls: ['./checkout-asset.component.css']
})
export class CheckoutAssetComponent implements OnInit {
  private readonly unsubscribe = new Subject<void>();

  @Input() card: LibraryCardForDetailedDto;
  public basketItems$: Observable<Checkout[]> = of([]);
  public basketItems: Checkout[] = [];
  dataSource = new MatTableDataSource<LibraryAsset>();
  displayedColumns = ['title', 'authorName', 'year', 'assetType', 'actions'];

  basket: BasketViewModel;
  searchForm = new FormGroup({
    searchString: new FormControl('', Validators.required)
  });

  constructor(private basketService: BasketService, private assetService: AssetService) {
    this.basketService.basket$.pipe(takeUntil(this.unsubscribe)).subscribe(basket => (this.basket = basket));
  }

  ngOnInit() {}

  searchAsset() {
    if (this.searchForm.valid) {
      this.assetService.searchAsset(this.searchForm.controls.searchString.value).subscribe((assets: LibraryAsset[]) => {
        this.dataSource.data = assets;
      });
    }
    this.dataSource.data = [];
  }

  onSearchClear() {
    this.searchForm.reset();
    this.dataSource.data = [];
  }

  addToCart(asset: LibraryAsset) {
    const checkout: Checkout = {};
    checkout.libraryAssetId = asset.id;
    checkout.asset = asset;
    checkout.userId = this.card.id;
    // this.basketService.addAssetToCart(checkout);
  }
}
