import { AfterViewInit, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { merge, Observable, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, takeUntil } from 'rxjs/operators';
import { PaginatedResult, Pagination } from 'src/app/_models/pagination';
import { AuthorDto, LibraryAssetForListDto } from 'src/dto/models';
import { AuthorService } from '../../services/author.service';

@Component({
  selector: 'lms-author-asset-list',
  templateUrl: './author-asset-list.component.html',
  styleUrls: ['./author-asset-list.component.css']
})
export class AuthorAssetListComponent implements AfterViewInit, OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  @Input()
  author: AuthorDto;
  @Input()
  assets!: PaginatedResult<LibraryAssetForListDto[]>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  dataSource = new MatTableDataSource<LibraryAssetForListDto>();
  displayedColumns = ['title', 'authorName', 'year', 'assetType', 'detail'];
  pagination!: Pagination;
  paginationOptions = new Pagination();
  searchString = new FormControl('');

  constructor(private readonly authorService: AuthorService) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngAfterViewInit(): void {
    merge(this.paginator.page, this.sort.sortChange)
      .pipe(
        takeUntil(this.unsubscribe),
        switchMap(() => this.getAssets())
      )
      .subscribe(paginatedCards => {
        this.mapPagination(paginatedCards);
      });
  }

  ngOnInit(): void {
    this.mapPagination(this.assets);
    this.searchAssets();
  }

  searchAssets(): void {
    this.searchString.valueChanges
      .pipe(
        takeUntil(this.unsubscribe),
        debounceTime(500),
        distinctUntilChanged(),
        switchMap(() => this.getAssets())
      )
      .subscribe(paginatedResult => this.mapPagination(paginatedResult));
  }

  onSearchClear(): void {
    this.searchString.setValue('');
  }

  private getAssets(): Observable<PaginatedResult<LibraryAssetForListDto[]>> {
    return this.authorService.getAssetsForAuthors(
      this.author.id,
      this.paginator.pageIndex + 1,
      this.paginator.pageSize,
      this.sort.active,
      this.sort.direction.toString(),
      this.searchString.value
    );
  }

  private mapPagination(result: PaginatedResult<LibraryAssetForListDto[]>): void {
    this.dataSource.data = result.result;
    this.pagination = result.pagination;
  }
}
