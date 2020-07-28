<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version = "1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:template match = "bookstore">
        <html lang="en">
            <body>
                <table style="font-size: 15px; border: 1px solid rgb(221, 221, 221); margin: 0 auto; border-collapse: collapse">
                    <tr>
                        <th style = "padding: 0.8em; border: 1px solid rgb(221, 221, 221);">title</th>
                        <th style = "padding: 0.8em; border: 1px solid rgb(221, 221, 221);">author</th>
                        <th style = "padding: 0.8em; border: 1px solid rgb(221, 221, 221);">category</th>
                        <th style = "padding: 0.8em; border: 1px solid rgb(221, 221, 221);">year</th>
                        <th style = "padding: 0.8em; border: 1px solid rgb(221, 221, 221);">price</th>
                    </tr>
                    <xsl:for-each select = "book">
                        <tr style = "font-size: 1em;">
                            <td style="border: 1px solid rgb(221, 221, 221);padding: 0.8em; "><xsl:value-of select = "title"/></td>
                            <td style="border: 1px solid rgb(221, 221, 221);padding: 0.8em; max-width:300px;"><xsl:for-each select = "author"> <span style = "margin-right: 5px; display: inline-block">
                                                                                                                    <xsl:value-of select = "."/>;</span>
                                                                                                                </xsl:for-each></td>
                            <td style="border: 1px solid rgb(221, 221, 221);padding: 0.8em; "><xsl:value-of select = "./@category"/></td>
                            <td style="border: 1px solid rgb(221, 221, 221);padding: 0.8em; "><xsl:value-of select = "year"/></td>
                            <td style="border: 1px solid rgb(221, 221, 221);padding: 0.8em; "><xsl:value-of select = "price"/></td>
                        </tr>
                    </xsl:for-each>
                </table>
            </body>
        </html>
    </xsl:template>
</xsl:stylesheet>