/// <binding BeforeBuild='all' Clean='cleanjs, cleansass' />
// include plug-ins
var gulp = require('gulp');
//var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var sass = require('gulp-sass');
var rename = require('gulp-rename');

var config = {
    //Include all js files but exclude any min.js files
    jsSrc: ['./wwwroot/js/**/*.js', '!./wwwroot/js/**/*.min.js'],
    sassSrc: ['./wwwroot/sass/**/*.scss', './wwwroot/sass/*.scss'],
    concatJsDest: ['./wwwroot/js/*.min.js'],
    cssSrc: ['./wwwroot/css/**/*.css'],
    cssDest: "./wwwroot/css",
    jsDest: "./wwwroot/js",
}

//delete the output file(s)
gulp.task('cleanjs', function () {
    //del is an async function and not a gulp plugin (just standard nodejs)
    //It returns a promise, so make sure you return that from this task function
    //  so gulp knows when the delete is complete
    return del(config.concatJsDest);
});

gulp.task('cleansass', function () {
    //del is an async function and not a gulp plugin (just standard nodejs)
    //It returns a promise, so make sure you return that from this task function
    //  so gulp knows when the delete is complete
    return del(config.cssSrc);
});

// Combine and minify all files from the app folder
// This tasks depends on the clean task which means gulp will ensure that the 
// Clean task is completed before running the scripts task.
gulp.task('min:js',  ['cleanjs'], function () {

    return gulp.src(config.jsSrc)
      .pipe(uglify())
      .pipe(rename({ suffix: '.min' }))
      .pipe(gulp.dest(config.jsDest));
});

gulp.task("sass", ['cleansass'], function () {
    return gulp.src(config.sassSrc)
      .pipe(sass({ outputStyle: 'compressed' }))
        .pipe(rename({ suffix: '.min' }))
      .pipe(gulp.dest(config.cssDest));
});

//Set a default tasks
gulp.task('all', ['min:js', 'sass'], function () { });

//gulp.task('watch', function () {
//    return gulp.watch(config.src, ['scripts']);
//});
