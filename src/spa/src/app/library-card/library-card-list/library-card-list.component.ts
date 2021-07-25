import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { merge, Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { Pagination } from 'src/app/_models/pagination';
import { NotificationService } from 'src/app/_services/notification.service';
import { LibrarycardForListDto } from 'src/dto/models';
import { LibraryCardComponent } from '../library-card/library-card.component';
import { LibraryCardService } from '../services/library-card.service';

@Component({
  templateUrl: './library-card-list.component.html',
  styleUrls: ['./library-card-list.component.css']
})
export class LibraryCardListComponent implements AfterViewInit, OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  cards: LibrarycardForListDto[] = [];
  pagination: Pagination;
  dataSource = new MatTableDataSource<LibrarycardForListDto>(this.cards);
  searchString = '';
  displayedColumns = ['libraryCardNumber', 'firstName', 'lastName', 'email', 'actions'];
  paginationOptions = new Pagination();
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private readonly cardService: LibraryCardService,
    private readonly route: ActivatedRoute,
    private readonly notify: NotificationService,
    public dialog: MatDialog
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(data => {
      this.pagination = data.cards.pagination;
      this.cards = data.cards.result;
      this.dataSource = new MatTableDataSource<LibrarycardForListDto>(this.cards);
    });
  }

  ngAfterViewInit(): void {
    merge(this.paginator.page, this.sort.sortChange)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(() => {
        this.loadData();
      });
  }

  filterList(): void {
    this.searchString.trim().toLocaleLowerCase();
    this.loadData();
  }

  onSearchClear(): void {
    this.searchString = '';
    this.filterList();
  }

  private getDialogConfig(): MatDialogConfig<any> {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '640px';

    return dialogConfig;
  }

  updateCard(element: any): void {
    const dialogConfig = this.getDialogConfig();
    dialogConfig.data = element;
    this.dialog.open(LibraryCardComponent, dialogConfig);
  }

  openAddCardDialog(): void {
    const dialogConfig = this.getDialogConfig();

    this.dialog.open(LibraryCardComponent, dialogConfig);
  }

  deleteCard(card: LibrarycardForListDto): void {
    this.notify
      .confirm('Are you sure you sure you want to delete this member')
      .afterClosed()
      .subscribe(res => {
        if (res) {
          this.cardService.deleteCard(card.id).subscribe(
            () => {
              this.cards.splice(
                this.cards.findIndex(x => x.id === card.id),
                1
              );
              this.notify.success('Member was deleted successfully');
              this.pagination.totalItems--;
              this.dataSource = new MatTableDataSource<LibrarycardForListDto>(this.cards);
            },
            error => {
              this.notify.error(error);
            }
          );
        }
      });
  }

  loadData(): void {
    this.cardService
      .getCards(
        this.paginator.pageIndex + 1,
        this.paginator.pageSize,
        this.sort.active,
        this.sort.direction.toString(),
        this.searchString
      )
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(
        cards => {
          this.cards = cards.result;
          this.dataSource = new MatTableDataSource<LibrarycardForListDto>(this.cards);
        },
        error => {
          this.notify.error(error);
        }
      );
  }
}
