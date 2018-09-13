<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
   xmlns:xsl="http://www.w3.org/1999/XSL/Transform" >
  <xsl:variable name="and"><![CDATA[&]]></xsl:variable>
  <xsl:output method="html" omit-xml-declaration="yes" />

  <!-- / forward slash is used to denote a patern that matches the root node of the XML document -->
  <xsl:template match ="/" >
    <html>
      <head>
        <title>
          <xsl:value-of select="ReviewReport/@Title" disable-output-escaping="yes"/>
        </title>
        <style>
          <xsl:text>
                    <![CDATA[
                    body
                    {
                    background:lightgrey;                    
                    margin-left: 3%;
                    }

                    .Tableheader
                    {
                    background: lightslategray;
                    color: white;
                    text-shadow: 1px 1px 1px springgreen;
                    text-align:center;
                    font-size:18 px;
                    width:95%;
                    border-radius: 15px;
                    cursor:pointer;
                    }

                    table
                    {
                    font-family: "Trebuchet MS", sans-serif;
                    font-size: 14px;
                    font-weight: bold;
                    line-height: 1.4em;
                    font-style: normal;
                    border-collapse:separate;
                    width:95%;
                    }

                    thead th{
                    padding:15px;
                    color:#fff;
                    text-shadow:1px 1px 1px #568F23;
                    border:1px solid #93CE37;
                    border-bottom:3px solid #9ED929;
                    background-color:#9DD929;
                    background:-webkit-gradient(
                    linear,
                    left bottom,
                    left top,
                    color-stop(0.02, rgb(123,192,67)),
                    color-stop(0.51, rgb(139,198,66)),
                    color-stop(0.87, rgb(158,217,41))
                    );
                    background: -moz-linear-gradient(
                    center bottom,
                    rgb(123,192,67) 2%,
                    rgb(139,198,66) 51%,
                    rgb(158,217,41) 87%
                    );
                    -webkit-border-top-left-radius:5px;
                    -webkit-border-top-right-radius:5px;
                    -moz-border-radius:5px 5px 0px 0px;
                    border-top-left-radius:5px;
                    border-top-right-radius:5px;
                    }

                    tfoot tr{
                    padding:15px;
                    color:#fff;
                    text-shadow:1px 1px 1px #568F23;
                    border:1px solid #93CE37;
                    border-bottom:3px solid #9ED929;
                    background-color:#9DD929;
                    background:-webkit-gradient(
                    linear,
                    left bottom,
                    left top,
                    color-stop(0.02, rgb(123,192,67)),
                    color-stop(0.51, rgb(139,198,66)),
                    color-stop(0.87, rgb(158,217,41))
                    );
                    background: -moz-linear-gradient(
                    center bottom,
                    rgb(123,192,67) 2%,
                    rgb(139,198,66) 51%,
                    rgb(158,217,41) 87%
                    );
                    -webkit-border-top-left-radius:5px;
                    -webkit-border-top-right-radius:5px;
                    -moz-border-radius:5px 5px 0px 0px;
                    border-top-left-radius:5px;
                    border-top-right-radius:5px;
                    }

                    thead th:empty{
                    background:transparent;
                    border:none;
                    }

                    tbody td{
                    padding:10px;
                    text-align:left;
                    background-color:#DEF3CA;
                    border: 2px solid #E7EFE0;
                    -moz-border-radius:2px;
                    -webkit-border-radius:2px;
                    border-radius:2px;
                    color:#666;                    
                    }

                    td:hover {
                    background-color: lightgray;
                    }
                    <!--tr:first-child {
                    background-color: black;
                    font-size:14 px;
                    font-color:white;
                    }

                    tr:nth-child(even) {
                    background-color: #ccc;
                    }

                    tr:hover {
                    background:yellow; }-->
                ]]>
                </xsl:text>
        </style>
      </head>
      <body>
        <script language="javascript" type="text/javascript">
          <xsl:text disable-output-escaping="yes" >
                    <![CDATA[
                    function toggle(element)
                    {
                        if(element)
                        {
                            var divToBeToggled = element.nextSibling;

                            for(var i = 1;i < 10; i++)
                            {
                               if(divToBeToggled)
                               {
                                if(divToBeToggled.nodeType == 1)
                                    break;
                                else
                                    divToBeToggled = divToBeToggled.nextSibling;
                               }
                             }

                            if(divToBeToggled)
                            {
                                if(divToBeToggled.style.display != "none")
                                    divToBeToggled.style.display = "none";
                                else
                                    divToBeToggled.style.display = "block";
                            }
                        }
                    }
                ]]>
                </xsl:text>
        </script>
        <center>
          <h1>
            <xsl:value-of select="ReviewReport/@Title" disable-output-escaping="yes"/>
          </h1>
        </center>
        <xsl:apply-templates />
      </body>
    </html>
  </xsl:template>

  <xsl:template match="table">
    <div style="clear:both;"/>
    <div style="clear:both;">
      <div style="clear:both;" class="Tableheader" onclick="toggle(this);">
        <xsl:value-of select="@title" disable-output-escaping="yes"/>
      </div>
      <div style="clear:both;">
        <table>
          <thead>
            <tr>
              <xsl:for-each select="col" >
                <th>
                  <xsl:value-of select="@name" />
                </th>
              </xsl:for-each>
            </tr>
          </thead>

          <xsl:for-each select="row" >
            <tr>
              <xsl:for-each select="col" >
                <td>
                  <xsl:value-of select="." />
                </td>
              </xsl:for-each>
            </tr>
          </xsl:for-each>

          <xsl:if test="@footer">
            <xsl:variable name="recordCount" select="count(col)"/>
            <tfoot>
              <tr>
                <td colspan="{$recordCount}">
                  <xsl:value-of select="@footer" disable-output-escaping="yes"/>
                </td>
              </tr>
            </tfoot>
          </xsl:if>

        </table>
      </div>
    </div>
  </xsl:template >


  <xsl:template match="label">
    <div style="clear:both;"/>
    <div style="clear:both;"/>
    <div style="clear:both;">
      <!--<xsl:value-of select="." disable-output-escaping="yes"/>-->
      <span>
        <xsl:value-of select="@name" />
        <xsl:choose>
          <xsl:when test="@name != ''">:</xsl:when>
        </xsl:choose>
        <xsl:value-of select="." />
      </span>
    </div>
  </xsl:template >

  <xsl:template match="Paragraph">
    <div style="clear:both;">
      <h5>
        <xsl:value-of select="@SubTitle" disable-output-escaping="yes"/>
      </h5>
      <p>
        <xsl:value-of select="." disable-output-escaping="yes"/>
      </p>
    </div>
  </xsl:template >

  <xsl:template match="Header">
    <div style="clear:both;height:15px;"></div>
    <h4>
      <xsl:value-of select="." disable-output-escaping="yes"/>
    </h4>
  </xsl:template >

  <xsl:template match="Footer">
    <div style="clear:both;"/>
    <div style="clear:both;"/>
    <div style="clear:both;height:10px;"></div>
    <div style="clear:both; ">
      <xsl:value-of select="." disable-output-escaping="yes"/>
    </div>
  </xsl:template >


</xsl:stylesheet >