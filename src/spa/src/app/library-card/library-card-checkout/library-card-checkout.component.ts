import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
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
export class LibraryCardCheckoutComponent implements OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @Input() card: LibraryCardForDetailedDto;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  assets: LibraryAssetForListDto[] = [];
  dataSource = new MatTableDataSource<LibraryAssetForListDto>(this.assets);
  displayedColumns = ['title', 'authorName', 'year', 'assetType', 'actions'];
  paginationOptions = new Pagination();
  searchString = new FormControl('', Validators.required);
  pagination: Pagination;

  constructor(
    private readonly basketService: BasketService,
    private readonly assetService: AssetService,
    private readonly notify: NotificationService
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  searchAssets() {
    this.searchString.valueChanges
      .pipe(
        takeUntil(this.unsubscribe),
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(value => this.getAssets(value))
      )
      .subscribe(
        assets => {
          this.pagination = assets.pagination;
          this.assets = assets.result;
          this.dataSource = new MatTableDataSource<LibraryAssetForListDto>(this.assets);
        },
        error => {
          this.notify.error('Problem retrieving data');
        }
      );
  }

  clearSearch() {
    this.searchString.setValue('');
    this.assets = [];
  }

  getAssets(searchString: string): Observable<PaginatedResult<LibraryAssetForListDto[]>> {
    return this.assetService.getPaginatedAssets(
      this.paginator.pageIndex + 1,
      this.paginator.pageSize,
      this.sort.active,
      this.sort.direction.toString(),
      searchString
    );
  }

  addToCart(asset: LibraryAssetForListDto): void {
    this.basketService.addAssetToCart(asset, this.card);
  }
}
