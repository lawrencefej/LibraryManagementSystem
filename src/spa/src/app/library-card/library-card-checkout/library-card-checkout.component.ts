import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { merge, Observable, of, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
import { BasketViewModel } from 'src/app/main/basket/models/basket-view-model';
import { LibraryAssetForBasketViewModel } from 'src/app/main/basket/models/library-asset-for-basket-view-model';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { AssetService } from 'src/app/_services/asset.service';
import { BasketService } from 'src/app/_services/basket.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { LibraryCardForDetailedDto, LibraryAssetForListDto } from 'src/dto/models';

@Component({
  selector: 'lms-library-card-checkout',
  templateUrl: './library-card-checkout.component.html',
  styleUrls: ['./library-card-checkout.component.css']
})
export class LibraryCardCheckoutComponent implements OnDestroy, OnInit, AfterViewInit {
  private readonly unsubscribe = new Subject<void>();

  @Input()
  card!: LibraryCardForDetailedDto;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  basket!: BasketViewModel;
  currentCheckedoutAssets: LibraryAssetForBasketViewModel[] = [];
  dataSource = new MatTableDataSource<LibraryAssetForListDto>();
  displayedColumns = ['title', 'authorName', 'year', 'assetType', 'actions'];
  pagination: Pagination;
  paginationOptions = new Pagination();
  searchString = new FormControl('');

  constructor(
    private readonly basketService: BasketService,
    private readonly assetService: AssetService,
    private readonly notify: NotificationService
  ) {}

  ngAfterViewInit(): void {
    merge(this.paginator.page, this.sort.sortChange)
      .pipe(
        takeUntil(this.unsubscribe),
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(() => this.getAssets(this.searchString.value))
      )
      .subscribe(paginatedCheckouts => {
        this.mapPagination(paginatedCheckouts);
      });
  }

  ngOnInit(): void {
    this.basketService.basket$.pipe(takeUntil(this.unsubscribe)).subscribe(basket => {
      this.basket = basket;
      this.mapCurrentCheckedoutAssets();
    });
    this.searchAssets().subscribe(assets => this.mapPagination(assets));
  }

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  stopCheckouts(newAsset: LibraryAssetForListDto): boolean {
    return !this.basket.active || this.basket.libraryCardId !== this.card.id || this.shouldAssetBeDenied(newAsset);
  }

  searchAssets() {
    return this.searchString.valueChanges.pipe(
      takeUntil(this.unsubscribe),
      debounceTime(500),
      distinctUntilChanged(),
      switchMap(value => this.getAssets(value))
    );
  }

  clearSearch() {
    this.searchString.setValue('');
    this.dataSource.data = [];
  }

  addToCart(newAsset: LibraryAssetForListDto): void {
    if (this.card.id !== this.basket.libraryCardId) {
      this.notify.error('Please initialize the for for the current card first.');
      return;
    }

    if (this.card.checkouts.find(asset => asset.libraryAssetId === newAsset.id)) {
      this.notify.error(`${newAsset.title} is currently checked out by the card.`);
      return;
    }

    if (this.currentCheckedoutAssets.find(asset => asset.libraryAssetId === newAsset.id)) {
      this.notify.error(`${newAsset.title} has already been placed in the basket.`);
      return;
    }

    if (this.currentCheckedoutAssets.length === 10) {
      this.notify.error(`${newAsset.title} puts the card over the maximum checkout limit.`);
      return;
    }

    this.basketService.addAssetToCart(newAsset, this.card);
  }

  private shouldAssetBeDenied(newAsset: LibraryAssetForListDto): boolean {
    if (this.card.id !== this.basket.libraryCardId) {
      return true;
    }

    if (this.card.checkouts.find(asset => asset.libraryAssetId === newAsset.id)) {
      return true;
    }

    if (this.currentCheckedoutAssets.find(asset => asset.libraryAssetId === newAsset.id)) {
      return true;
    }

    if (this.currentCheckedoutAssets.length === 10) {
      return true;
    }

    return false;
  }

  private getAssets(searchString: string): Observable<PaginatedResult<LibraryAssetForListDto[]>> {
    const test = new PaginatedResult<LibraryAssetForListDto[]>();
    test.result = [];
    if (searchString === '') {
      return of(test);
    } else {
      return this.assetService.getPaginatedAssets(
        this.paginator.pageIndex + 1,
        this.paginator.pageSize,
        this.sort.active,
        this.sort.direction.toString(),
        searchString
      );
    }
  }

  private mapPagination(result: PaginatedResult<LibraryAssetForListDto[]>) {
    this.dataSource.data = result.result;
    this.pagination = result.pagination;
  }

  private mapCurrentCheckedoutAssets(): void {
    this.currentCheckedoutAssets = this.card.checkouts.map(a => ({
      libraryAssetId: a.libraryAssetId,
      title: a.title,
      author: ''
    }));
    this.currentCheckedoutAssets = [...this.currentCheckedoutAssets, ...this.basket.assets];
  }
}
