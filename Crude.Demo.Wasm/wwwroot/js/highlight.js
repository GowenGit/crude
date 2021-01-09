function Highlight() {
    // first, find all the div.code blocks
    document.querySelectorAll('div pre code').forEach(block => {
        // then highlight each
        hljs.highlightBlock(block);
    });
}