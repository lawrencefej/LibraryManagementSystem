import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, of } from 'rxjs';

import { AssetService } from 'src/app/_services/asset.service';
import { BasketService } from 'src/app/_services/basket.service';
import { Checkout } from 'src/app/_models/checkout';
import { LibraryAsset } from 'src/app/_models/libraryAsset';
import { MatTableDataSource } from '@angular/material/table';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'lms-checkout-asset',
  templateUrl: './checkout-asset.component.html',
  styleUrls: ['./checkout-asset.component.css']
})
export class CheckoutAssetComponent implements OnInit {
  @Input() member: User;
  public basketItems$: Observable<Checkout[]> = of([]);
  public basketItems: Checkout[] = [];
  dataSource = new MatTableDataSource<LibraryAsset>();
  basketNumber = 0;
  displayedColumns = ['title', 'authorName', 'year', 'assetType', 'actions'];
  searchForm = new FormGroup({
    searchString: new FormControl('', Validators.required)
  });

  constructor(private basketService: BasketService, private assetService: AssetService) {
    this.basketItems$ = this.basketService.getItemsInBasket();

    this.basketItems$.subscribe(_ => (this.basketItems = _));
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
    checkout.userId = this.member.id;
    this.basketService.addAssetToCart(checkout);
  }
}
