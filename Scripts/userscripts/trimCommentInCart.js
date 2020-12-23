try {
    var length = App.orderComment.value.length;

    if (length > 100) {
        App.orderComment.setValue(App.orderComment.value.substring(0, length - 1));  
        App.symbolsCount.setText('Осталось 0 символов из 100');
    }
    else {
        App.symbolsCount.setText('Осталось ' + (100 - length) + ' символов из 100');
    }
}
catch {

}