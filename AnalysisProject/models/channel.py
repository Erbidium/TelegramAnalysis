from sqlalchemy import Boolean
from sqlalchemy.orm import Mapped
from sqlalchemy.orm import mapped_column
from models.base import Base
from sqlalchemy import String

class Channel(Base):
     __tablename__ = "channels"

     id: Mapped[int] = mapped_column(primary_key=True)
     mainusername: Mapped[str] = mapped_column(String())
     title: Mapped[str] = mapped_column(String())
     isdeleted: Mapped[bool] = mapped_column(Boolean())


     def repr(self) -> str:
         return f"Channel(id={self.id!r}, mainusername={self.mainusername!r}, title={self.title!r}, isdeleted={self.isdeleted!r})"
