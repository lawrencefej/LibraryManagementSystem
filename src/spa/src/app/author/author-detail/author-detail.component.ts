import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { PaginatedResult } from 'src/app/_models/pagination';
import { AuthorDto, LibraryAssetForListDto } from 'src/dto/models';
import { AuthorComponent } from '../author/author.component';
import { AuthorService } from '../services/author.service';

@Component({
  templateUrl: './author-detail.component.html',
  styleUrls: ['./author-detail.component.css']
})
export class AuthorDetailComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  author!: AuthorDto;
  assetsForAuthor!: PaginatedResult<LibraryAssetForListDto[]>;

  constructor(
    private readonly route: ActivatedRoute,
    private readonly dialog: MatDialog,
    private readonly authorService: AuthorService
  ) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => {
      this.author = routeData.initData.author;
      this.assetsForAuthor = routeData.initData.assets;
    });
  }

  editAuthor(): void {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.width = '500px';
    dialogConfig.data = this.author;
    const dialogRef: MatDialogRef<AuthorComponent, AuthorDto> = this.dialog.open(AuthorComponent, dialogConfig);

    dialogRef
      .afterClosed()
      .pipe(takeUntil(this.unsubscribe))
      .subscribe(returnAuthor => (this.author = returnAuthor));
  }
}
