try {
    if (App.orderComment.text.length > 100) {
        App.orderComment.setText(App.orderComment.text.substring(0, 100) + '...')
    }
    else {

    }
}
catch {

}