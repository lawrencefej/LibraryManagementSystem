import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { PaginatedResult } from 'src/app/_models/pagination';
import { AuthorDto, LibraryAssetForListDto } from 'src/dto/models';

@Component({
  templateUrl: './author-detail.component.html',
  styleUrls: ['./author-detail.component.css']
})
export class AuthorDetailComponent implements OnInit, OnDestroy {
  private readonly unsubscribe = new Subject<void>();

  author!: AuthorDto;
  assetsForAuthor!: PaginatedResult<LibraryAssetForListDto[]>;

  constructor(private readonly route: ActivatedRoute) {}

  ngOnDestroy(): void {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  ngOnInit(): void {
    this.route.data.pipe(takeUntil(this.unsubscribe)).subscribe(routeData => {
      this.author = routeData.initData.author;
      this.assetsForAuthor = routeData.initData.assets;
      console.log(routeData);
    });
  }
}
